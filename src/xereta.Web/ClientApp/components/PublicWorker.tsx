import * as React from 'react';

export interface PublicWorkerData {
    id: string,
    name: string,
    cpf: string,
    siape: string,
    originDepartment: string,
    workingDepartment: string,
    role: string,
    salaries: Salaries[]
}

interface Salaries {
    year: number,
    month: number,
    day: number,
    income: number
}

export class PublicWorker extends React.Component<any, any>{
    public render(){
        return <div>
                <p>Nome: {this.props.publicWorkerData.name}</p>
                <p>CPF: {this.props.publicWorkerData.cpf}</p>
                <p>Poder: Executivo</p>
                {this.printSalary()}
                <input type='button' value='Voltar' onClick={this.props.handleBackButton}/>
              </div>;
    }

    private printSalary(){
        if (this.props.publicWorkerData.salaries.length > 0)
            return <p>{'Salário: R$'} {this.props.publicWorkerData.salaries[0].income}</p>
        else
            return <p>{'Não há informação de salário registrada deste servidor.'}</p>
    }
}