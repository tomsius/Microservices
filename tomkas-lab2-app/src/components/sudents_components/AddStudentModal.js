import React, { Component } from 'react';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Form from "react-bootstrap/Form";
import Snackbar from '@mui/material/Snackbar';
import IconButton from '@mui/material/IconButton';

export class AddStudentModal extends Component {
    constructor(props) {
        super(props);

        this.state = {
            snackbaropen: false,
            snackbarmessage: "",
            firstName: "",
            lastName: "",
            date: ""
        };

        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleSubmit(event) {
        event.preventDefault();

        fetch('http://localhost:5083/students',
            {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    'FirstMidName': this.state.firstName,
                    'LastName': this.state.lastName,
                    'EnrollmentDate': this.state.date,
                    'Enrollments': []
                })
            })
            .then((response) => {
                if (response.ok) {
                    this.props.onAdd();
                    return response.json();
                }

                throw new Error(response.status + " " + response.statusText);
            })
            .then(result => {
                this.setState({
                    snackbaropen: true,
                    snackbarmessage: "Studentas \"" + this.state.firstName + " " + this.state.lastName + "\" buvo pridėtas.",
                    firstName: "",
                    lastName: "",
                    date: ""
                });
            },
                (error) => {
                    this.setState({
                        snackbaropen: true,
                        snackbarmessage: "Nepavyko pridėti studento: " + error.message
                    });
                });
    }

    render() {
        return (
            <div className="container">
                <Snackbar
                    anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
                    open={this.state.snackbaropen}
                    autoHideDuration={3000}
                    onClose={() => this.setState({ snackbaropen: false })}
                    message={<span id="message-id">{this.state.snackbarmessage}</span>}
                    action={[<IconButton key="close" arial-label="Close" color="inherit" onClick={() => this.setState({ snackbaropen: false })}>
                        x
                    </IconButton>]}
                />

                <Modal
                    {...this.props}
                    size="lg"
                    aria-labelledby="contained-modal-title-vcenter"
                    centered
                >
                    <Modal.Header closeButton>
                        <Modal.Title id="contained-modal-title-vcenter">
                            Pridėti naują studentą
                        </Modal.Title>
                    </Modal.Header>
                    <Modal.Body>

                        <Row>
                            <Col sm={6}>
                                <Form onSubmit={this.handleSubmit}>
                                    <Form.Group controlId="FirstName">
                                        <Form.Label>Studento vardas</Form.Label>
                                        <Form.Control type="text" name="FirstName" required placeholder="Studento vardas" value={this.state.firstName} onChange={e => this.setState({ firstName: e.target.value })} />
                                    </Form.Group>
                                    <Form.Group controlId="LastName">
                                        <Form.Label>Studento pavardė</Form.Label>
                                        <Form.Control type="text" name="LastName" required placeholder="Studento pavardė" value={this.state.lastName} onChange={e => this.setState({ lastName: e.target.value })} />
                                    </Form.Group>
                                    <Form.Group controlId="EnrollmentDate">
                                        <Form.Label>Registracijos data</Form.Label>
                                        <Form.Control type="date" name="EnrollmentDate" required value={this.state.date} onChange={e => this.setState({ date: e.target.value })} />
                                    </Form.Group>
                                    <Form.Group>
                                        <Button variant="primary" type="submit">
                                            Pridėti
                                        </Button>
                                    </Form.Group>
                                </Form>
                            </Col>
                        </Row>

                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="danger" onClick={this.props.onHide}>Uždaryti</Button>
                    </Modal.Footer>
                </Modal>
            </div>
        );
    }
}