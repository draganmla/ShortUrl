import React, { Component } from 'react';

export default class ShortenUrl extends Component {
    static displayName = ShortenUrl.name;

    constructor(props) {
        super(props);
        this.state = {
            LongUrlValue: "",
            ShortenedUrl: ""
        }
    }

    componentDidMount() {

    }

    handleChange(e) {
        this.setState({ LongUrlValue: e.target.value });
    }

    onClickSubmit = () => {
        const url = process.env.REACT_APP_API_URL

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ LongURL: this.state.LongUrlValue })
        };
        console.log(url);
        fetch(url, requestOptions)
            .then(response => response.json())
            .then(data => this.setState({ LongUrlValue: data.model.longURL, ShortenedUrl: data.model.shortURL }));
    }
    render() {

        return (
            <div>
                <div>
                    <input
                        type="text"
                        value={this.state.LongUrlValue}
                        onChange={this.handleChange.bind(this)}
                        placeholder="Write a URL..."
                    />
                    <button onClick={this.onClickSubmit}>
                        Submit
                    </button>
                </div>
                <div><a href={process.env.REACT_APP_API_URL + "/" + this.state.ShortenedUrl}>{"http://drgn.ly/" + this.state.ShortenedUrl}</a></div>
            </div>
        )
    }
}
