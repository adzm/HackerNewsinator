import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Subscription } from 'rxjs';
import { Article } from '../interfaces/article';

@Injectable({
  providedIn: 'root'
})
export class ArticlesService {

  http: HttpClient;
  baseUrl: string;

  loadingFresh: Subscription = null;
  loadingSearch: Subscription = null;

  articles: Article[];

  searchText: string;
  errorText: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  ngOnDestroy() {
    if (this.loadingFresh) {
      this.loadingFresh.unsubscribe();
      this.loadingFresh = null;
    }
    if (this.loadingSearch) {
      this.loadingSearch.unsubscribe();
      this.loadingSearch = null;
    }
  }

  getArticles() {
    return this.articles;
  }

  loadFreshArticles(after?: number) {
    this.searchText = null;
    let queryParams = new HttpParams();
    if (after != undefined) {
      queryParams = queryParams.append('after', after.toString());      
    }
    this.loadingFresh = this.http.get<Article[]>(this.baseUrl + 'Articles', { params: queryParams }).subscribe(result => {
      this.errorText = null;
      this.loadingFresh.unsubscribe();
      this.loadingFresh = null;
      if (after == undefined) {
        this.articles = result;
      } else {
        this.articles = this.articles.concat(result);
      }
    }, error => {
      this.loadingFresh.unsubscribe();
      this.loadingFresh = null;
      console.error(error);
      this.errorText = error.message;
    });
  }

  getLastArticle(): number {
    const count = this.articles.length;
    if (count) {
      return this.articles[count - 1].id;
    } else {
      return undefined;
    }
  }

  nextPage() {
    if (this.searchText) {
      if (this.loadingSearch) {
        return;
      }
      if (this.loadingFresh) {
        this.loadingFresh.unsubscribe();
        this.loadingFresh = null;
      }
      let after = this.getLastArticle();
      this.searchArticles(this.searchText, after);
    } else {
      if (this.loadingFresh) {
        return;
      }
      if (this.loadingSearch) {
        this.loadingSearch.unsubscribe();
        this.loadingSearch = null;
      }
      let after = this.getLastArticle();
      this.loadFreshArticles(after);
    }
  }

  searchArticles(searchText: string, after?: number) {
    this.searchText = searchText;
    let queryParams = new HttpParams();
    queryParams = queryParams.append('q', searchText);
    if (after != undefined) {
      queryParams = queryParams.append('after', after.toString());
    }
    this.loadingSearch = this.http.get<Article[]>(this.baseUrl + 'Articles/Search', { params: queryParams }).subscribe(result => {
      this.errorText = null;
      this.loadingSearch.unsubscribe();
      this.loadingSearch = null;
      if (after == undefined) {
        this.articles = result;
      } else {
        this.articles = this.articles.concat(result);
      }
    }, error => {
      this.loadingSearch.unsubscribe();
      this.loadingSearch = null;
      console.error(error);
      this.errorText = error.message;
    });
  }
}
