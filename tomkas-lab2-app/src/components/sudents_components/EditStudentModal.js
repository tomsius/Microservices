import React, { Component } from 'react'
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Form from "react-bootstrap/Form";
import Snackbar from '@mui/material/Snackbar';
import IconButton from '@mui/material/IconButton';

export class EditStudentModal extends Component {
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

        fetch('http://localhost:5083/students/' + this.props.studentId,
            {
                method: 'PUT',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    'ID': this.props.studentId,
                    'FirstMidName': this.state.firstName === "" ? this.props.studentFirstName : this.state.firstName,
                    'LastName': this.state.lastName === "" ? this.props.studentLastName : this.state.lastName,
                    'EnrollmentDate': this.state.date === "" ? this.props.studentEnrollmentDate : this.state.date,
                    'Enrollments': []
                })
            })
            .then((response) => {
                if (response.ok) {
                    this.props.onEdit();
                    this.setState({
                        snackbaropen: true,
                        snackbarmessage: "Studento duomenys atnaujinti.",
                        firstName: "",
                        lastName: "",
                        date: ""
                    });

                    return;
                }
                else {
                    throw new Error(response.status + " " + response.statusText);
                }
            })
            .then(() => { },
                (error) => {
                    this.setState({
                        snackbaropen: true,
                        snackbarmessage: "Nepavyko atnaujinti studento duomenų: " + error.message
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
                            Redaguoti studento duomenis
                        </Modal.Title>
                    </Modal.Header>
                    <Modal.Body>

                        <Row>
                            <Col sm={6}>
                                <Form onSubmit={this.handleSubmit}>
                                    <Form.Group controlId="StudentId">
                                        <Form.Label>Studento ID</Form.Label>
                                        <Form.Control type="text" name="StudentId" disabled defaultValue={this.props.studentId} />
                                    </Form.Group>
                                    <Form.Group controlId="FirstName">
                                        <Form.Label>Studento vardas</Form.Label>
                                        <Form.Control type="text" name="FirstName" required placeholder="Studento vardas" defaultValue={this.props.studentFirstName} onChange={e => this.setState({ firstName: e.target.value })} />
                                    </Form.Group>
                                    <Form.Group controlId="LastName">
                                        <Form.Label>Studento pavardė</Form.Label>
                                        <Form.Control type="text" name="LastName" required placeholder="Studento pavardė" defaultValue={this.props.studentLastName} onChange={e => this.setState({ lastName: e.target.value })} />
                                    </Form.Group>
                                    <Form.Group controlId="EnrollmentDate">
                                        <Form.Label>Registracijos data</Form.Label>
                                        <Form.Control type="date" name="EnrollmentDate" required defaultValue={new Date(this.props.studentEnrollmentDate).toLocaleDateString('en-CA')} onChange={e => this.setState({ date: e.target.value })} />
                                    </Form.Group>
                                    <Form.Group>
                                        <Button variant="primary" type="submit">
                                            Atnaujinti
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