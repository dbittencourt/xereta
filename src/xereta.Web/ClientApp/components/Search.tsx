import * as React from 'react';
import 'isomorphic-fetch';
import { PublicWorker, PublicWorkerData } from './PublicWorker';


interface SearchDataState {
    query: string,
    searchResult: PublicWorkerData[],
    publicWorkers: PublicWorkerData[],
    loading: boolean,
    individualProfile: boolean
}

class SearchBar extends React.Component<any, void>{
    constructor(props){
        super(props);
        this.handleChange = this.handleChange.bind(this);
    }

    public render(){
        return  <div>
                    <label>Nome ou CPF: 
                        <input type='text' placeholder="Search..." value={this.props.query} 
                        onChange={this.handleChange}/>
                    </label>
                    <input type='button' value='search' onClick={this.props.handleClick}/>
                </div>;
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

    handleClick(id){
        this.props.onPublicWorkerClicked(id);
    }

    public render(){
        return <div><h3>Resultados</h3> 
            <table className="table">
                <thead>
                    <tr>
                        <th>Nome</th>
                        <th>{'Órgão de Origem'}</th>
                        <th>{'Órgão de Exercício'}</th>
                    </tr>
                </thead>
                <tbody>
                    {this.props.publicWorkers.map(publicWorker => 
                        <tr key={publicWorker.id} onClick={() => this.handleClick(publicWorker.id)}>
                            <td>{publicWorker.name}</td>
                            <td>{publicWorker.originDepartment}</td>
                            <td>{publicWorker.workingDepartment}</td>
                        </tr>
                    )}
                </tbody>
            </table></div>;
    }

}

export class Search extends React.Component<any, SearchDataState>{
    constructor(){
        super();
        this.state = {query: '', searchResult: [], loading: false, publicWorkers: [], individualProfile: false};
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

    private handleQueryChange(newQuery) {
        this.setState({query: newQuery, searchResult: this.state.searchResult, individualProfile: this.state.individualProfile,
            loading: this.state.loading, publicWorkers: this.state.publicWorkers});
    }

    private handleSearchClick(){
        this.setState({query: this.state.query, searchResult: this.state.searchResult, 
            loading: true, publicWorkers: this.state.publicWorkers, individualProfile: false});

        fetch('//localhost:5000/api/servidores?q=' + this.state.query)
        .then(response => response.json() as Promise<PublicWorkerData[]>)
        .then(newSearchResult => {
            this.setState({searchResult: newSearchResult, query: this.state.query, 
                loading: false, publicWorkers: this.state.publicWorkers, individualProfile: false});
        });
    }

    private getPublicWorkerData(id){
        fetch('//localhost:5000/api/servidores/' + id)
        .then(response => response.json() as Promise<PublicWorkerData>)
        .then(publicWorker => {
            this.setState({query: this.state.query, loading: this.state.loading, individualProfile: true, 
            searchResult: this.state.searchResult, publicWorkers: [publicWorker].concat(this.state.publicWorkers)});
        });
    }

    private handleBackButton(){
        this.setState({query: this.state.query, searchResult: this.state.searchResult, publicWorkers: this.state.publicWorkers,
                        loading: this.state.loading, individualProfile: false});
    }

    private renderContent(){
        if (this.state.individualProfile){
            var publicWorkerData = this.state.publicWorkers[0];
            return <PublicWorker publicWorkerData={publicWorkerData} handleBackButton={this.handleBackButton}/>

        }
        else {
            var hasResult = this.state.searchResult.length > 0;

            if (hasResult)
                return <SearchResult publicWorkers={this.state.searchResult} 
                                    onPublicWorkerClicked={this.getPublicWorkerData}/>;
            else {
                if (this.state.loading)
                    return <p><em>Loading...</em></p>;
            }
        }
    }
}