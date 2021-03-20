import React, { Component } from 'react';
import ShortenUrl from './ShortenUrl';

export class Home extends Component {
    static displayName = Home.name;

    render() {

        return (
            <div>
                <h1>Hello, world!</h1>
                <p>Welcome to your new single-page application, built with react:</p>

                <p><ShortenUrl /></p>
                <p>Shortened urls are generted from {process.env.REACT_APP_API_URL}</p>
            </div>
        );
    }
}
