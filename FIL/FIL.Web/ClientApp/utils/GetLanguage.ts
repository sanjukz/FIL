
export const GetLanguage = () => {
    let current_url = 'https://www.feelitlive.com/';
    if (typeof window !== undefined) {
        current_url = window.location.href;
    }
    let currentCookie = getCookie('googtrans');

    if (current_url.indexOf('.es') > -1) {
        // will set cookie only if language is not set
        if (currentCookie == null) {
            setCookie('googtrans', '/en/es')
        }
    }
    if (current_url.indexOf('.de') > -1) {
        // will set cookie only if language is not set
        if (currentCookie == null) {
            setCookie('googtrans', '/en/de')
        }
    }
}

function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}
function setCookie(name, value) {
    var expires = "";
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}