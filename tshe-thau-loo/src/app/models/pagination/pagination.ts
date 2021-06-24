export class Pagination {

  // tslint:disable-next-line: variable-name
  private readonly _maxPageSize: number = 50;

  // eslint-disable-next-line @typescript-eslint/member-ordering
  public pageLength!: number; // 總資料數

  // tslint:disable-next-line: variable-name
  private _pageSize: number;

  // tslint:disable-next-line: variable-name
  private _pageIndex: number;

  constructor() {
    // eslint-disable-next-line no-underscore-dangle
    this._pageIndex = 0;
    // eslint-disable-next-line no-underscore-dangle
    this._pageSize = 10;
  }

  // 目前頁碼 - 1
  public get pageIndex(): number {
    // eslint-disable-next-line no-underscore-dangle
    return this._pageIndex;
  }

  public set pageIndex(value: number) {
    if (value < 0) {
      // eslint-disable-next-line no-underscore-dangle
      this._pageIndex = 0;
    } else {
      // eslint-disable-next-line no-underscore-dangle
      this._pageIndex = value;
    }
  }

  // 一頁的項目數
  public get pageSize(): number {
    // eslint-disable-next-line no-underscore-dangle
    return this._pageSize;
  }

  public set pageSize(value: number) {
    // eslint-disable-next-line no-underscore-dangle
    if (value > this._maxPageSize) {
      // eslint-disable-next-line no-underscore-dangle
      this._pageSize = this._maxPageSize;
    } else {
      // eslint-disable-next-line no-underscore-dangle
      this._pageSize = value;
    }
  }

  public get page(): number {
    if (this.pageLength <= this.pageSize) {
      return 1;
    } else {
      return Math.ceil(this.pageLength / this.pageSize);
    }
  }

}
