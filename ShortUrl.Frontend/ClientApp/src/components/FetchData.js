import React, { Component } from 'react';

export class FetchData extends Component {
    static displayName = FetchData.name;

    constructor(props) {
        super(props);
        this.state = { urls: [], loading: true };
    }

    componentDidMount() {
        const url = process.env.REACT_APP_API_URL
        fetch(url).then(response => {
            return response.json()
        }).then(data => {
            this.setState({ urls: data, loading: false });
        })
    }

    static renderForecastsTable(urls) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Shortened Url</th>
                        <th>Full Url</th>
                    </tr>
                </thead>
                <tbody>
                    {urls.map(url =>
                        <tr key={url.id}>
                            <td><a href={process.env.REACT_APP_API_URL+"/" + url.shortURL}>{"http://drgn.ly/" + url.shortURL}</a></td>
                            <td>{url.longURL}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchData.renderForecastsTable(this.state.urls);

        return (
            <div>
                <h1 id="tabelLabel" >List of all shortened urls </h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>
        );
    }
}
