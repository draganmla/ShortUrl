import React, { Component } from 'react';

export default class ShortenUrl extends Component {
    static displayName = ShortenUrl.name;

    constructor(props) {
        super(props);
        this.state = {
            LongUrlValue: "",
            ShortenedUrl: "",
            Errors: null,
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

        fetch(url, requestOptions)
            .then(response => {
                if (response.ok) {
                    response.json()
                        .then(data => this.setState({ LongUrlValue: data.model.longURL, ShortenedUrl: data.model.shortURL, Errors: null }))
                } else {
                    response.json()
                        .then(data => data.errors).
                        then(errors => this.setState({ LongUrlValue: "", ShortenedUrl: "", Errors: JSON.stringify(errors) }))
                }
            })
    }
    render() {
        const { Errors } = this.state;

        return (
            <div>
                <div>
                    <input
                        type="text"
                        value={this.state.LongUrlValue}
                        onChange={this.handleChange.bind(this)}
                        placeholder="http://www.example.com"
                    />
                    <button onClick={this.onClickSubmit}>
                        Submit
                    </button>
                </div>
                { Errors ? <div>{this.state.Errors}</div> : <div><a href={process.env.REACT_APP_API_URL + "/" + this.state.ShortenedUrl}>{"http://drgn.ly/" + this.state.ShortenedUrl}</a></div>}
            </div>
        )
    }
}
