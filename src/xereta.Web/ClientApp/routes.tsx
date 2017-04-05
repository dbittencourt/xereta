import * as React from 'react';
import { Router, Route, HistoryBase } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Contact } from './components/Contact';

export default <Route component={ Layout }>
                    <Route path='/' components={{body: Home}} />
                    <Route path='/home' components={{body: Home}} />
                    <Route path= '/search' components={{body: Home}} />
                    <Route path='/contact' components={{body: Contact}} />
               </Route>;