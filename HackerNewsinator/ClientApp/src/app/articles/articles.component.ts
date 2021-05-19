import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ArticlesService } from '../services/articles-service';
import { Article } from '../interfaces/article';

@Component({
  selector: 'app-articles',
  templateUrl: './articles.component.html'
})
export class ArticlesComponent {
  get articles(): Article[] {
    return this.articlesService.getArticles();
  };

  get searchText(): string {
    return this.articlesService.searchText;
  }

  get errorText(): string {
    return this.articlesService.errorText;
  }

  articlesService: ArticlesService;

  constructor(articlesService: ArticlesService) {
    this.articlesService = articlesService;
    this.articlesService.loadFreshArticles();
  }

  onScroll() {
    this.articlesService.nextPage();
  }
}


