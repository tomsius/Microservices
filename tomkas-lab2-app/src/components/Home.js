import React, { Component } from 'react';

export class Home extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="container">
                <h1>Tomas Kašelynas IFM-1/2</h1>
                <h2>Individualus darbas</h2>
                <p>
                    Laboratorinis darbas nr. 2 buvo praplėstas:
                </p>
                <ol>
                    <li>Health monitoring.</li>
                    <li>Logging with Serilog and Seq.</li>
                    <li>SPA client.</li>
                </ol>
            </div>
        );
    }
}