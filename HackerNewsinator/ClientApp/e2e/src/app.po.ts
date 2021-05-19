import { browser, by, element } from 'protractor';

export class AppPage {
  navigateTo() {
    return browser.get('/');
  }

  navigateToAbout() {
    return browser.get('/about');
  }

  getLoadingIndicator() {
    return element(by.id('loadingText'));
  }

  getErrorIndicator() {
    return element(by.id('errorText'));
  }

  getMainHeaderText() {
    return element(by.css('app-root h1')).getText();
  }
}
