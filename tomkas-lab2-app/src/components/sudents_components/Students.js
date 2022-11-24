import React, { Component } from 'react';
import Table from 'react-bootstrap/Table';
import Button from 'react-bootstrap/Button';
import ButtonToolbar from 'react-bootstrap/ButtonToolbar';
import { AddStudentModal } from './AddStudentModal';
import { EditStudentModal } from './EditStudentModal';

export class Students extends Component {
    constructor(props) {
        super(props);
        this.state = {
            students: [],
            addModalShow: false,
            editModalShow: false
        };

        this.refreshList = this.refreshList.bind(this);
    }

    componentDidMount() {
        this.refreshList();
    }

    refreshList() {
        fetch("http://localhost:5083/students", { method: 'GET' })
            .then(response => response.json())
            .then(data => {
                this.setState({
                    students: data
                })
            });
    }

    deleteStudent(id, firstName, lastName) {
        if (window.confirm("Ar tikrai norite ištrinti studentą \"" + firstName + " " + lastName + "\"?")) {
            fetch("http://localhost:5083/students/" + id,
                {
                    method: 'DELETE'
                })
                .then((response) => {
                    this.refreshList();
                });
        }
    }

    render() {
        const { students, studentId, studentFirstName, studentLastName, studentEnrollmentDate } = this.state;
        let addModalClose = () => this.setState({ addModalShow: false });
        let editModalClose = () => this.setState({ editModalShow: false });

        return (
            <div className="container">
                <h1 style={{ textAlign: "center" }}>Studentų sąrašas</h1>
                <Table className="mt-4" striped bordered hover size="sm">
                    <thead>
                        <tr>
                            <th>Vardas</th>
                            <th>Pavardė</th>
                            <th>Registracijos data</th>
                            <th>Veiksmai</th>
                        </tr>
                    </thead>
                    <tbody>
                        {students.map(s =>
                            <tr key={s.id}>
                                <td>
                                    {s.firstMidName}
                                </td>
                                <td>
                                    {s.lastName}
                                </td>
                                <td>
                                    {s.enrollmentDate}
                                </td>
                                <td>
                                    <ButtonToolbar>
                                        <Button className="mr-2" variant="info" onClick={() => this.setState({ editModalShow: true, studentId: s.id, studentFirstName: s.firstMidName, studentLastName: s.lastName, studentEnrollmentDate: s.enrollmentDate })}>
                                            Redaguoti
                                        </Button>

                                        <Button className="mr-2" onClick={() => this.deleteStudent(s.id, s.firstMidName, s.lastName)} variant="danger">
                                            Ištrinti
                                        </Button>

                                        <EditStudentModal show={this.state.editModalShow} onHide={editModalClose}
                                            studentId={studentId}
                                            studentFirstName={studentFirstName}
                                            studentLastName={studentLastName}
                                            studentEnrollmentDate={studentEnrollmentDate}
                                            onEdit={this.refreshList}
                                        />
                                    </ButtonToolbar>
                                </td>
                            </tr>
                        )}
                    </tbody>
                </Table>
                <ButtonToolbar>
                    <Button variant="primary" onClick={() => this.setState({ addModalShow: true })}>
                        Naujas studentas
                    </Button>
                    <AddStudentModal show={this.state.addModalShow} onHide={addModalClose} onAdd={this.refreshList} />
                </ButtonToolbar>
            </div>
        );
    }
}