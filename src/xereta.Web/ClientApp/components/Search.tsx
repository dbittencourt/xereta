import * as React from 'react';
import 'isomorphic-fetch';
import { PublicWorker, PublicWorkerData } from './PublicWorker';
import { Button, Input, Table, Spin } from 'antd';


interface SearchDataState {
    query?: string,
    searchResult?: PublicWorkerData[],
    publicWorkers?: PublicWorkerData[],
    loading?: boolean,
    individualProfile?: boolean,
    publicWorkerDisplayed?: PublicWorkerData
}

class SearchBar extends React.Component<any, void>{
    constructor(props){
        super(props);
        this.handleChange = this.handleChange.bind(this);
    }

    public render(){
        return (
            <div id="searchBox">
                <Input.Search placeholder="Nome ou CPF" value={this.props.query} onChange={this.handleChange} 
                onPressEnter={this.props.handleClick} onSearch={this.props.handleClick}/>
            </div>
        );
    }

    private handleChange(event){
        this.props.handleChange(event.target.value);
    }
}

class SearchResult extends React.Component<any, void>{
    constructor(props){
        super(props);
        this.handleClick = this.handleClick.bind(this);
    }

    handleClick(searchResult){
        this.props.onPublicWorkerClicked(searchResult.id);
    }

    private createTable(searchResults){
        var state = {
            pagination: false,
            scroll: {y: 600}
        };
        var columns = [
            { title: 'Nome', dataIndex: 'name', key: 'name', width: 150 },
            { title: 'Órgão de Origem', dataIndex: 'originDepartment', key: 'originDepartment', width: 150 },
            { title: 'Órgão de Exercício', dataIndex: 'workingDepartment', key: 'workingDepartment', width: 150 }
        ];

        searchResults.map(function(searchResult){
            searchResult.key = searchResult.id;
        });

        return <Table {...state} columns={columns} dataSource={searchResults} onRowClick={this.handleClick} />
    }
    
    public render(){
        return (
            <div>
                {this.createTable(this.props.publicWorkers)}
            </div>
        );
    }

}

export class Search extends React.Component<any, SearchDataState>{
    constructor(){
        super();
        this.state = {query: '', searchResult: [], loading: false, publicWorkers: [], 
            individualProfile: false, publicWorkerDisplayed: null};
        this.handleQueryChange = this.handleQueryChange.bind(this);
        this.handleSearchClick = this.handleSearchClick.bind(this);
        this.getPublicWorkerData = this.getPublicWorkerData.bind(this);
        this.handleBackButton = this.handleBackButton.bind(this);
    }
    
    public render(){
        return <div>
                <SearchBar query={this.state.query} handleChange={this.handleQueryChange} handleClick={this.handleSearchClick}/>
                {this.renderContent()}
               </div>;
    } 

    private handleQueryChange(newQuery){
        this.setState({query: newQuery});
    }

    private handleSearchClick(){
        this.setState({loading: true});

        fetch('//localhost:5000/api/servidores?q=' + this.state.query)
        .then(response => response.json() as Promise<PublicWorkerData[]>)
        .then(newSearchResult => {
            this.setState({searchResult: newSearchResult, loading: false});
        });
    }

    private getPublicWorkerData(id){
        this.setState({loading: true});

        // verifies if the public worker was already retrieved
        var pWorker = this.state.publicWorkers.filter(function(publicWorker){
                                if (publicWorker.id == id)
                                    return publicWorker;
                                })[0];
        if (pWorker != null){
            this.setState({loading: false, individualProfile: true, publicWorkerDisplayed: pWorker});
        }
        else {
            fetch('//localhost:5000/api/servidores/' + id)
                .then(response => response.json() as Promise<PublicWorkerData>)
                .then(publicWorker => {
                    this.setState({loading: false, individualProfile: true, publicWorkerDisplayed: publicWorker,
                        publicWorkers: this.state.publicWorkers.concat(publicWorker)});
            });
        }
    }

    private handleBackButton(){
        this.setState({individualProfile: false});
    }

    private renderContent(){
        if (this.state.loading){
            return <Spin tip="Carregando..." />;
        }

        if (this.state.individualProfile){
            return <PublicWorker publicWorkerData={this.state.publicWorkerDisplayed} handleBackButton={this.handleBackButton}/>
        }

        var hasResult = this.state.searchResult.length > 0;
        if (hasResult)
            return <SearchResult publicWorkers={this.state.searchResult} 
                                onPublicWorkerClicked={this.getPublicWorkerData} />;
    }
}