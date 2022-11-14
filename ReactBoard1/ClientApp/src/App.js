import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';

import './custom.css'
import EntryIndex from "./components/Entries/EntryIndex";
import EntryCreate from "./components/Entries/EntryCreate";
import EntryEdit from "./components/Entries/EntryEdit";
import EntryDetails from "./components/Entries/EntryDetails";
import EntryDelete from "./components/Entries/EntryDelete";

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path={['/entries', '/entries/index']} component={EntryIndex} exact />
        <Route path='/entries/create' component={EntryCreate} />
        <Route path='/entries/edit/:id' component={EntryEdit} />
        <Route path='/entries/details/:id' component={EntryDetails} />
        <Route path='/entries/delete/:id' component={EntryDelete} />
        <AuthorizeRoute path='/fetch-data' component={FetchData} />
        <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
      </Layout>
    );
  }
}
