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
                <p>
                    Kiti servisai:
                </p>
                <ul>
                    <li><a href='http://localhost:5081/swagger/index.html'>TomKasCoursesAPI</a></li>
                    <li><a href='http://localhost:5082/swagger/index.html'>TomKasStudentsAPI</a></li>
                    <li><a href='http://localhost:5084/'>TomKasHealthMonitor</a></li>
                    <li><a href='http://localhost:5085/'>Seq</a></li>
                </ul>
            </div>
        );
    }
}