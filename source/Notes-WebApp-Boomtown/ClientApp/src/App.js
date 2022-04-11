import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { EditNote } from './components/EditNote';
import NoteView  from './views/NoteView';
import CreateView from './views/CreateView';
import 'bootstrap/dist/css/bootstrap.min.css';
import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
      return (
        <Layout>
            <Switch>
                  <Route exact path='/' component={Home} />
                  <Route exact path='/note/create' component={CreateView} />
                  <Route path='/counter' component={Counter} />
                  <Route path='/fetch-data' component={FetchData} />
                  <Route path='/note/:id' component={NoteView} />
            </Switch>
         </Layout>
    );
  }
}
