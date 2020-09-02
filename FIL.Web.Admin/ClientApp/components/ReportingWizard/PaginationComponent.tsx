import * as React from "react";

export default class PaginationComponent extends React.Component<any, any>{
    constructor(props) {
        super(props);
    }
    public render() {
        
        var rowList = [20, 25, 50, 100, 200, 500, 1000];
        var currentRow = parseInt(this.props.noRecordsPerPage);
        var rowOptions = rowList.map(function(val){
                return val == currentRow ? <option value={val} selected>{val} rows</option> : <option value={val}>{val} rows</option>;
        }); 
        var totalPages = 0, prevPages = 0, nextPages = 0;
        if (parseInt(this.props.totalRecords) > 0) {
            totalPages = parseInt(this.props.totalRecords) / parseInt(this.props.noRecordsPerPage);
            totalPages = parseInt(totalPages.toString());
            var isPage = parseInt(this.props.totalRecords) % parseInt(this.props.noRecordsPerPage);
            if (isPage > 0) {
                totalPages = totalPages + 1;
            }
            if (totalPages == 0) {
                totalPages = 1;
            }

            prevPages = parseInt(this.props.pageNo) - 2;
            nextPages = parseInt(this.props.pageNo) + 2;
            var tempPageNo = parseInt(this.props.pageNo);
            if (totalPages > 0) {
                if (prevPages <= 0) {
                    var start_inc = 1 - (prevPages);

                    if (nextPages < totalPages) {
                        nextPages = nextPages + start_inc;
                        if (nextPages > totalPages) {
                            nextPages = totalPages;
                        }
                    }
                    prevPages = 1;
                }
                var prevButton, pageListButton =[], nextButton;
                if (nextPages > totalPages) {
                    var end_inc = nextPages - (totalPages);

                    if (prevPages >= 0) {
                        prevPages = prevPages - end_inc;
                        if (prevPages < 1) {
                            prevPages = 1;
                        }
                    }
                    nextPages = totalPages;
                }
                if (totalPages > 1) {
                    if (parseInt(this.props.pageNo) == 1) {
                        prevButton = <li className="page-item disabled"><a href="javascript:void(0);" className="page-link">« Prev</a> </li>;
                    }
                    else {
                        prevButton = <li className="page-item"><a onClick={this.props.ShowRecords.bind(this, this.props.pageNo - 1)} href="javascript:void(0);" className="page-link">« Prev</a> </li>;
                    }

                    while (prevPages >= 1 && prevPages < parseInt(this.props.pageNo)) {
                        pageListButton.push(<li className="page-item"><a className="page-link" onClick={this.props.ShowRecords.bind(this, prevPages)} href="javascript:void(0);">{prevPages}</a> </li>);
                        prevPages++;

                    }
                    pageListButton.push(<li className="page-item active" ><a className="page-link">{this.props.pageNo}</a> </li>);
                    while (nextPages <= totalPages && nextPages > parseInt(this.props.pageNo)) {
                        tempPageNo = tempPageNo + 1;
                        pageListButton.push(<li className="page-item"><a onClick={this.props.ShowRecords.bind(this, tempPageNo)} className="page-link" href="javascript:void(0);">{tempPageNo}</a> </li>);
                        nextPages--;
                    }

                    if (parseInt(this.props.pageNo) == totalPages) {
                        nextButton = <li className="page-item disabled"><a className="page-link" href="javascript:void(0);">Next »</a> </li>;
                    }
                    else {
                        nextButton = <li className="page-item"><a onClick={this.props.ShowRecords.bind(this, parseInt(this.props.pageNo) + 1)} className="page-link" href="javascript:void(0);">Next »</a> </li>;
                    }
                }
            }

            return ( <div>
                <div className="row">
                    <div className="col-sm-6 text-right">
                        <div className="form-group">
                            <span> Page </span>
                            <input className="form-control input-sm pagination-input" type="number" placeholder="Page" value={this.props.pageNo} onChange={this.props.OnChangeNoPaginationInput.bind(this, totalPages)}/>
                           <span> of {totalPages} </span>
                            <select className="form-control input-sm pagination-input" onChange={this.props.OnChangeNoRecordsPerPage.bind(this)}>
                                {rowOptions}
                            </select>
                        </div>
                    </div>
                    <div className="col-8">
                        <div className="form-group">
                            <nav aria-label="Page navigation" className="text-right pr-20">
                                <ul className="pagination m-0">
                                    {prevButton}
                                    {pageListButton}
                                    {nextButton}
                                </ul>
                            </nav>
                        </div>
                    </div>
                </div>
            </div>);
        }
    }
}

