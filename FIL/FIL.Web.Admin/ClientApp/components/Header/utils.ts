export function getMyAccountLink() {
  switch (window.location.origin) {
    case 'https://host.static5.feelitlive.com':
      return 'https://www.static5.feelitlive.com/account';
    case 'https://admin.static5.feelitlive.com':
      return 'https://www.static5.feelitlive.com/account';
    case 'https://host.feelitlive.com':
      return 'https://www.feelitlive.com/account';
    case 'https://admin.feelitlive.com':
      return 'https://www.feelitlive.com/account';
    default:
      return 'https://dev.feelitlive.co.in/account';
  }
}
