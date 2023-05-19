﻿using FOAEA3.Model;
using FOAEA3.Model.Base;
using FOAEA3.Model.Interfaces;
using System;

namespace BackendProcesses.Business
{
    public class DivertFundsProcess
    {
        private readonly IRepositories Repositories;
        private readonly IRepositories_Finance RepositoriesFinance;
        private readonly DateTime _currentTime = DateTime.Now;

        public DivertFundsProcess(IRepositories repositories, IRepositories_Finance repositoriesFinance)
        {
            Repositories = repositories;
            RepositoriesFinance = repositoriesFinance;
        }

        public void Run(string source = "", string FABatchID = "")
        {
            var prodAudit = Repositories.ProductionAuditRepository;

            prodAudit.Insert("Divert Funds", "Divert Funds Started", "O");

            var controlBatchList = GetDAFAReadyBatches(source, FABatchID);
            if (controlBatchList.Items.Count > 0)
            {
                ExecDivertFunds(controlBatchList);
                prodAudit.Insert("Divert Funds", "Divert Funds Completed", "O");
            }
            else
                prodAudit.Insert("Divert Funds", "Divert Funds - No FA Ready Bacthes", "O");

        }

        private void ExecDivertFunds(DataList<ControlBatchData> controlBatchList)
        {
            bool _flagErrorInDFBatch = false;
            var prodAudit = Repositories.ProductionAuditRepository;

            foreach (var rowDAFABatch in controlBatchList.Items)
            {
                _flagErrorInDFBatch = false;
                string _DFBatchID = GetDFBatchID(rowDAFABatch);

                prodAudit.Insert("Divert Funds", $"Divert Funds Batch Created - {_DFBatchID} Time: {DateTime.Now}", "O");

                var transactionList = GetSummFaFrDEBatches(rowDAFABatch);

                // process all FRs first
                foreach (var rowTransaction in transactionList.Items)
                {
                    // initialize variables

                    try
                    {

                        var _debtorId = rowTransaction.Dbtr_Id;
                        if (rowTransaction.SummFAFR_OrigFA_IndDesc == "FR")
                        {
                            ProcessFRBatches(rowTransaction.SummFAFR_Id);
                        }

                    }
                    catch (Exception e)
                    {
                        _flagErrorInDFBatch = true;
                        // InsertNightlyProcessTransactionError(rowTransaction.SummFaFr_Id, _summFAFRId, _DFBatchID, DateTime.Now(), _methodName, ex.ToString())
                    }
                }

                // process all FAs second
                foreach (var rowTransaction in transactionList.Items)
                {
                    // initialize variables

                    try
                    {

                        var _debtorId = rowTransaction.Dbtr_Id;
                        //???                        if (rowTransaction.SummFAFR_OrigFA_IndDesc == "FR")
                        {
                            // ExecuteDFCalc(rowTransaction)
                        }

                    }
                    catch (Exception e)
                    {
                        _flagErrorInDFBatch = true;
                        // InsertNightlyProcessTransactionError(rowTransaction.SummFaFr_Id, _summFAFRId, _DFBatchID, DateTime.Now(), _methodName, ex.ToString())
                    }
                }

            }

            /*
             
             Dim transactionList As Justice.FOAEA.Common.TransactionListForBatch


        _errorMsg = New StringCollection

        For Each rowDAFABatch In _batchList.ControlBatch
            _flagErrorInDFBatch = False
            _DFBatchID = GetDFBatchID(rowDAFABatch)

            InsertProductionAudit("Divert Funds", "Divert Funds Batch Created - " & _DFBatchID & " Time: " & DateTime.Now().ToString, Nothing, Nothing)

            transactionList = GetSummFaFrDEBatches(rowDAFABatch)

            For Each rowTransaction In transactionList.GetTransactionListForBatch
                'Try
                InitializeMemberVariables()


                Try
                    'Step 3.4.1
                    _debtorId = rowTransaction.Dbtr_Id

                    'Step 3.4.2
                    _methodName = "Execute"
                    'Throw New DivideByZeroException
                    If (rowTransaction.SummFAFR_OrigFA_IndDesc = "FR") Then
                        ProcessFRBatches(rowTransaction.SummFaFr_Id)
                    End If

                    _methodName = ""

                Catch ex As Exception
                    _flagErrorInDFBatch = True

                    InsertNightlyProcessTransactionError(rowTransaction.SummFaFr_Id, _summFAFRId, _DFBatchID, DateTime.Now(), _methodName, ex.ToString())
                End Try
            Next


            For Each rowTransaction In transactionList.GetTransactionListForBatch
                'Try
                InitializeMemberVariables()
                            
                Try
                    _debtorId = rowTransaction.Dbtr_Id

                    _methodName = "Execute"
                    ExecuteDFCalc(rowTransaction)
                    _methodName = ""

                Catch ex As Exception
                    _flagErrorInDFBatch = True
                    '_errorCounter = _errorCounter + 1

                    'Dim _errorMsg As String = "SummFaFr_Id = " & rowTransaction.SummFaFr_Id & " " & "summFAFRId = " & _summFAFRId & " " & "_DFBatchID = " & _DFBatchID & " " & _
                    '                         "Time = " & DateTime.Now() & " " & "Method Name = " & _methodName & " " & "ex.StackTrace = " & ex.ToString()

                    'Dim divertFundsException As New FOAEA.Exceptions.DivertFundsException("911")           '(_errorMsg)
                    'ExceptionPolicy.HandleException(ex, "WF Exception Shield", divertFundsException)
                    'ExceptionPolicy.HandleException(ex, "General Logging Handler")
                    'ExceptionPolicy.HandleException(ex, "WF Exception Shield", divertFundsException)

                    InsertNightlyProcessTransactionError(rowTransaction.SummFaFr_Id, _summFAFRId, _DFBatchID, DateTime.Now(), _methodName, ex.ToString())
                    'The code bellow will be implemented if needed.
                    'If (_errorCounter > 5) Then
                    'stop the execution of the nightly process ???
                    'End If
                End Try
            Next
            If _errorMsg.Count > 0 Then
                Dim emailBody As String = ""
                For Each msg As String In _errorMsg
                    emailBody = emailBody & msg
                Next
                'CR 570 - uncomment for future use
                With New Justice.FOAEA.MidTier.Business.LoginSystem(_connectionStringFOAEA)
                    .SendEmail("CRA file received - Ongoing arrears negative amount(s) created in DF batch " + rowDAFABatch.DataEntryBatch_Id, System.Configuration.ConfigurationManager.AppSettings("EmailRecipients"), emailBody)
                End With
                _errorMsg.Clear()
            End If

            'Step # ??? - do we need this step
            'InsertMsgIntoProdAudit("DF", "Calc Engine Df", Nothing, Nothing)

            'Step 3.5
            'Dim DAFABatchlistDT As Justice.FOAEA.Common.ControlBatchData.ControlBatchDataTable = PrepDaFaCtrlBatch(rowDAFABatch)
            PrepDaFaCtrlBatch(rowDAFABatch)
            UpdateCtrlBatch(_batchList.ControlBatch)

            'Step 3.6
            Dim tempCtrlBatchList As New Justice.FOAEA.Common.ControlBatchData
            tempCtrlBatchList.ControlBatch.AddControlBatchRow(tempCtrlBatchList.ControlBatch.NewControlBatchRow)

            GetDFBatchSpecsDebtr(_DFBatchID, tempCtrlBatchList)

            'Step # ??? - do we need this step
            'InsertMsgIntoProdAudit("DF", "Update DA / FA Ctrl Batch", Nothing, Nothing)

            'Step 3.7
            GetDFBatchSpecsAppl(_DFBatchID, tempCtrlBatchList)

            'Step 3.8
            'Dim CRControlBatch As Justice.FOAEA.Common.ControlBatchData.ControlBatchDataTable = New Justice.FOAEA.Common.ControlBatchData.ControlBatchDataTable
            'CRControlBatch = 
            Dim tempDFBatchList As Justice.FOAEA.Common.ControlBatchData = PrepDFCtrlBatch(_DFBatchID, tempCtrlBatchList)
            UpdateCtrlBatch(tempDFBatchList.ControlBatch)

            'Step # ??? - do we need this step
            'InsertMsgIntoProdAudit("DF", "UpdateDF Ctrl Batch", Nothing, Nothing)
        Next
             
             */
        }

        private void ProcessFRBatches(int summFAFR_Id)
        {
            var summFAFR_DE_DB = RepositoriesFinance.SummFAFR_DERepository;
            var summFAFR_DB = RepositoriesFinance.SummFAFRRepository;
            var summDF_DB = RepositoriesFinance.SummDFRepository;

            var summFAFR_DE_List = summFAFR_DE_DB.GetSummFaFrDe(summFAFR_Id);
            var summFAFR_List = summFAFR_DB.GetSummFaFrList(summFAFR_DE_List.Items);
            //var summDF_List = new DataList<SummDF_Data>();

            foreach (var rowSummFAFR in summFAFR_List.Items)
            {
                var summDF_List = summDF_DB.GetSummDFList(rowSummFAFR.SummFAFR_Id);


            }

            /*
            Private Sub ProcessFRBatches(ByVal summFaFrId As Integer)

                'Step 1.3 
                For Each rowSummFaFr In summFaFrList.SummFAFR
                    'Step 1.3.1  
                    summDF_List = GetSummDFList(rowSummFaFr.SummFAFR_Id)

                    'Step 1.3.2
                    UpdateSummSmryForFundReversal(rowSummFaFr, summDF_List)

                    'Step 1.3.3
                    UpdateSummFaFr(rowSummFaFr.SummFAFR_Id)

                    'Step 1.3.4
                    DeleteSummDF(rowSummFaFr.SummFAFR_Id)

                    'Step 1.3.5
                    DeleteGarnPeriod(rowSummFaFr.Appl_EnfSrv_Cd, rowSummFaFr.Appl_CtrlCd, rowSummFaFr.SummFAFR_Id)

                    'Prepare for next For-Each iteration
                    summDF_List.Clear()
                Next
            End Sub

             */
        }


        private DataList<SummFAFR_DE_Data> GetSummFaFrDEBatches(ControlBatchData rowDAFABatch)
        {
            var summFAFR_DE_DB = RepositoriesFinance.SummFAFR_DERepository;

            return summFAFR_DE_DB.GetSummFaFrDeReadyBatches(rowDAFABatch.EnfSrv_Src_Cd, rowDAFABatch.Batch_Id);
        }

        public DataList<ControlBatchData> GetDAFAReadyBatches(string source = "", string FABatchID = "")
        {
            var controlBatchDB = RepositoriesFinance.ControlBatchRepository;

            return controlBatchDB.GetFADAReadyBatch(source, FABatchID);
        }

        private string GetDFBatchID(ControlBatchData rowDAFABatch)
        {
            var controlBatchDB = RepositoriesFinance.ControlBatchRepository;

            var values = new ControlBatchData()
            {
                Batch_Id = string.Empty,
                EnfSrv_Src_Cd = rowDAFABatch.EnfSrv_Src_Cd,
                DataEntryBatch_Id = rowDAFABatch.DataEntryBatch_Id,
                BatchType_Cd = "DF",
                Batch_Post_Dte = _currentTime,
                Batch_Compl_Dte = null,
                Medium_Cd = rowDAFABatch.Medium_Cd,
                SourceRecCnt = 0,
                DoJRecCnt = 0,
                SourceTtlAmt_Money = 0M,
                DoJTtlAmt_Money = 0M,
                BatchLiSt_Cd = 1,
                Batch_Reas_Cd = null,
                Batch_Pend_Ind = 0,
                PendTtlAmt_Money = 0M
            };

            controlBatchDB.CreateXFControlBatch(values, out _, out string batchID, out _, out _);

            return batchID;

        }

    }

}
