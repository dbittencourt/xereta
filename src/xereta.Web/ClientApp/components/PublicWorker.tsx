import * as React from 'react';
import { Button, Card, Select } from 'antd';

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

const meses = [ 'zero', 'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 
                'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];

export class PublicWorker extends React.Component<any, any>{
    constructor(){
        super();
        this.handleChange = this.handleChange.bind(this);
    }
    
    public render(){
        return <Card title={this.props.publicWorkerData.name}>
                    <p><b>CPF:</b> {this.props.publicWorkerData.cpf}</p>
                    <p><b>Poder:</b> Executivo</p>
                    <div className="container">
                        {this.printSalarySelect()}
                        <div className="salaryDisplay"><p id="salaryValue">R$ {this.props.publicWorkerData.salaries[0].income}</p></div>
                    </div>
                    <Button onClick={this.props.handleBackButton}>Voltar</Button>
              </Card>
    }

    private handleChange(value){
        document.getElementById("salaryValue").innerText = "R$ " + value;
    }

    private printSalarySelect(){
        if (this.props.publicWorkerData.salaries.length > 0){
            return (<div className="salaryDisplay">
                        <Select onChange={this.handleChange}>
                            {this.props.publicWorkerData.salaries.map((sal) =>
                                <Select.Option value={sal.income}>{meses[sal.month]} {sal.year}</Select.Option>)}   
                        </Select>
                    </div>);
        }
        else
            return <p>{'Não há informação de salário registrada deste servidor.'}</p>
    }
}