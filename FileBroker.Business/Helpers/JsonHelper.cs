using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace FileBroker.Business.Helpers
{
    public class JsonHelper
    {
        public static List<string> Validate<T>(string source, out List<UnknownTag> unknownTags)
        {
            var result = new List<string>();
            unknownTags = [];

            var generator = new JSchemaGenerator
            {
                DefaultRequired = Newtonsoft.Json.Required.Default // fields are NOT mandatory, by default
            };

            var schema = generator.Generate(typeof(T));
            try
            {
                var sourceData = JObject.Parse(source);
                bool isValid = sourceData.IsValid(schema, out IList<string> errors);

                foreach (string error in errors)
                {
                    string errorMessage = error;
                    
                    if (error.Contains("Invalid type. Expected Array"))
                    {
                        errorMessage = string.Empty; // ignore these errors, the json conversion should handle them
                    }
                    else if (error.Contains("has not been defined"))
                    {
                        var unknownTag = ExtractUnknownTagFromErrorMessage(error);
                        unknownTags.Add(unknownTag);
                        errorMessage = string.Empty; // ignore these errors, the extra tag will be listed as warnings
                    }

                    if (!string.IsNullOrEmpty(errorMessage))
                        result.Add(errorMessage);
                }
            }
            catch (Exception e)
            {
                result.Add(e.Message);
            }

            return result;
        }

        private static UnknownTag ExtractUnknownTagFromErrorMessage(string error)
        {
            // unknown tags will generate an error like this:
            //   Property 'extraTag' has not been defined and the schema does not allow additional properties. Path 'NewDataSet.TRCAPPIN20[3].extraTag', line 1, position 3058.

            var items = error.Split("'");
            if (items.Length > 3)
            {
                string thisTag = items[1];
                string thisSection = items[3];
                var sectionItems = thisSection.Split(".");
                if (sectionItems.Length > 1)
                    thisSection = sectionItems[1];

                return new UnknownTag
                {
                    Tag = thisTag,
                    Section = thisSection
                };
            }

            return new UnknownTag { Tag = error };
        }
    }
}
