interface ICustomRegexObject {
    ex: RegExp; //Regex expression
    msg: string; //message in case the test failed
}

interface ICustomRegex {
    [key: string] : ICustomRegexObject
}

export const CUSTOM_REGEX: ICustomRegex = {
    onlyString: {
        ex: /^([a-zA-z\s]{3,32})$/, 
        msg: "Only alphabetical values are allowed"
    },
    email: {
        ex: /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/,
        msg: "This doesn't seem like a email. Please check."
    },
    alphanumeric: {
        ex: /^([A-Za-z]|[A-Za-z][0-9]*|[0-9]*[A-Za-z])+$/,
        msg: "Only alphabets and numbers are allowed."
    },
    phoneNumber: {
        ex: /^[0-9]+$/,
        msg: "Something is wrong with the phone number. Please re-enter."
    }
};