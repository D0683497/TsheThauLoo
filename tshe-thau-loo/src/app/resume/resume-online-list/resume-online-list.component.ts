import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-resume-online-list',
  templateUrl: './resume-online-list.component.html',
  styleUrls: ['./resume-online-list.component.scss'],
})
export class ResumeOnlineListComponent implements OnInit {

  date = Date.now();
  archive = false;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    if (this.route.snapshot.queryParamMap.has('archive')) {
      this.archive = this.route.snapshot.queryParamMap.get('archive').toLocaleLowerCase() === 'true';
    }
  }

}
