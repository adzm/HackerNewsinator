import { Component } from '@angular/core';
import { ArticlesService } from '../services/articles-service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  searchText: string = "";
  articlesService: ArticlesService;

  constructor(articlesService: ArticlesService) {
    this.articlesService = articlesService;
  }

  search(text: string) {
    this.searchText = text;
    this.articlesService.searchArticles(text);
  }
  searchChanged(text: string) {
    if (!text) {
      this.searchText = "";
      this.articlesService.loadFreshArticles();
    }
  }
}
