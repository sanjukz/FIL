import { FeelUserReport } from "../models/Report/FeeluserReportResponseViewModel";

export const filteredTestUsers = (users: FeelUserReport[]) => {
  let filteredData = users.filter((item) => {
    return (
      checkIfPresent(item.firstName.toLocaleLowerCase()) &&
      checkIfPresent(item.lastName.toLocaleLowerCase()) &&
      checkIfPresent(item.email.toLocaleLowerCase()) &&
      checkIfPresent(item.firstName.toLocaleLowerCase())
    )
  });
  return filteredData;
}

export const filteredTestTransactions = (data: any) => {
  if (data && data.length > 0) {
    data = data.filter((item) => {
      return (
        checkIfPresent(item.customerEmail ? item.customerEmail.toLocaleLowerCase() : '') &&
        checkIfPresent(item.customerName ? item.customerName.toLocaleLowerCase() : '')
      )
    })
  }
  return data;
}


const test_users = ['test', 'pratibha.dhage@zoonga.com', 'vivekonline', 'shashikant.pandey'];

function checkIfPresent(inputString) {
  let flag = true;
  test_users.map((item) => {
    if (inputString.indexOf(item) > -1) {
      flag = false;
    }
  })
  return flag;
}


export const getTotalCount = (data: any, type: string, showTestUsers: boolean) => {
  let filteredData = [];
  if (!showTestUsers) {
    filteredData = data.filter((item) => {
      return (
        checkIfPresent(item.firstName.toLocaleLowerCase()) &&
        checkIfPresent(item.lastName.toLocaleLowerCase()) &&
        checkIfPresent(item.email.toLocaleLowerCase()) &&
        checkIfPresent(item.firstName.toLocaleLowerCase())
      )
    });
  }
  filteredData = data.filter((item) => {
    return item.signUpMethod == type
  });
  return filteredData.length;
}
