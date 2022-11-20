import React, { Component } from 'react';

export class NotFound extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="container">
                <h1 style={{ paddingTop: 50 }}>Toks puslapis neegzistuoja.</h1>
                <a href="/">Atgal į pradžią</a>
            </div>
        );
    }
}