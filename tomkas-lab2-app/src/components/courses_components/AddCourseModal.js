import React, { Component } from 'react';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Form from "react-bootstrap/Form";
import Snackbar from '@mui/material/Snackbar';
import IconButton from '@mui/material/IconButton';

export class AddCourseModal extends Component {
    constructor(props) {
        super(props);

        this.state = {
            snackbaropen: false,
            snackbarmessage: "",
            id: 0,
            title: "",
            credits: 0
        };

        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleSubmit(event) {
        event.preventDefault();

        fetch('http://localhost:5083/courses',
            {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    'CourseID': this.state.id,
                    'Title': this.state.title,
                    'Credits': this.state.credits
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
                    snackbarmessage: "Kursas \"" + this.state.title + " (" + this.state.credits + " kr.)\" buvo pridėtas.",
                    id: 0,
                    title: "",
                    credits: 0
                });
            },
                (error) => {
                    this.setState({
                        snackbaropen: true,
                        snackbarmessage: "Nepavyko pridėti kurso: " + error.message
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
                            Pridėti naują kursą
                        </Modal.Title>
                    </Modal.Header>
                    <Modal.Body>

                        <Row>
                            <Col sm={6}>
                                <Form onSubmit={this.handleSubmit}>
                                    <Form.Group controlId="CourseId">
                                        <Form.Label>ID</Form.Label>
                                        <Form.Control type="number" min={0} name="CourseId" required placeholder="ID" value={this.state.id} onChange={e => this.setState({ id: e.target.value })} />
                                    </Form.Group>
                                    <Form.Group controlId="Title">
                                        <Form.Label>Pavadinimas</Form.Label>
                                        <Form.Control type="text" name="Title" required placeholder="Kurso pavadinimas" value={this.state.title} onChange={e => this.setState({ title: e.target.value })} />
                                    </Form.Group>
                                    <Form.Group controlId="Credits">
                                        <Form.Label>Kreditai</Form.Label>
                                        <Form.Control type="number" min={0} name="Credits" required placeholder="Kreditai" value={this.state.credits} onChange={e => this.setState({ credits: e.target.value })} />
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