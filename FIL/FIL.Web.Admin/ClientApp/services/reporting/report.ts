import axios from "axios";
import { ReportResponseDataViewModel } from "../../models/ReportingWizard/ReportResponseDataViewModel";
import { InventoryReportResponseDataViewModel } from "../../models/ReportingWizard/InventoryReportResponseDataViewModel";
import { ScanningReportResponseViewModel } from "../../models/ReportingWizard/ScanningReportResponseViewModel";
import TransactionReportResponseViewModel from "../../models/Report/TransactionReportResponseViewModel";

function getTransactionReport(values) {
    return axios.post("api/report/transaction", values, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<ReportResponseDataViewModel>)
        .catch((error) => {
        });
}

function getTransactionReportDataAsSingleDataModelReturn(values) {
    return axios.post("api/get/reports", values, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<TransactionReportResponseViewModel>)
        .catch((error) => {
            console.log(error);
        });
}


function getFailedTransactionReport(values) {
    return axios.post("api/report/failedtransaction", values, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<ReportResponseDataViewModel>)
        .catch((error) => {
        });
}

function getInventoryReport(values) {
    return axios.post("api/report/inventory", values, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<InventoryReportResponseDataViewModel>)
        .catch((error) => {
        });
}

function getScanningReport(values) {
    return axios.post("api/report/scanning", values, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<ScanningReportResponseViewModel>)
        .catch((error) => {
        });
}

export const reportService = {
    getTransactionReport,
    getFailedTransactionReport,
    getTransactionReportDataAsSingleDataModelReturn,
    getInventoryReport,
    getScanningReport
};