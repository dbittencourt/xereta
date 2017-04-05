import * as React from 'react';
import { Search } from './Search';

export class Home extends React.Component<any, void>{
    public render(){
        return <div>
                    <h2>{'Xereta - Pesquisa fácil no portal transparência'}</h2>
                    <br />
                    <p>Aqui é possível visualizar, de forma simples e rápida, o salário dos servidores públicos brasileiros!</p>
                    <br />
                    <Search />
               </div>
    }

}