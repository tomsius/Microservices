import React, { Component } from 'react';
import Table from 'react-bootstrap/Table';
import Button from 'react-bootstrap/Button';
import ButtonToolbar from 'react-bootstrap/ButtonToolbar';
import { AddCourseModal } from './AddCourseModal';
import { EditCourseModal } from './EditCourseModal';

export class Courses extends Component {
    constructor(props) {
        super(props);
        this.state = {
            courses: [],
            addModalShow: false,
            editModalShow: false
        };
    }

    componentDidMount() {
        this.refreshList();
    }

    refreshList() {
        fetch("http://localhost:5083/courses", { method: 'GET' })
            .then(response => response.json())
            .then(data => {
                this.setState({
                    courses: data
                })
            });
    }


    deleteCourse(id, title) {
        if (window.confirm("Ar tikrai norite ištrinti kursą \"" + title + "\"?")) {
            fetch("http://localhost:5083/courses/" + id,
                {
                    method: 'DELETE'
                })
        }
    }

    componentDidUpdate() {
        this.refreshList();
    }

    render() {
        const { courses, courseId, courseTitle, courseCredits } = this.state;
        let addModalClose = () => this.setState({ addModalShow: false });
        let editModalClose = () => this.setState({ editModalShow: false });

        return (
            <div className="container">
                <h1 style={{ textAlign: "center" }}>Kursų sąrašas</h1>
                <Table className="mt-4" striped bordered hover size="sm">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Pavadinimas</th>
                            <th>Kreditai</th>
                            <th>Veiksmai</th>
                        </tr>
                    </thead>
                    <tbody>
                        {courses.map(c =>
                            <tr key={c.courseID}>
                                <td>
                                    {c.courseID}
                                </td>
                                <td>
                                    {c.title}
                                </td>
                                <td>
                                    {c.credits}
                                </td>
                                <td>
                                    <ButtonToolbar>
                                        <Button className="mr-2" variant="info" onClick={() => this.setState({ editModalShow: true, courseId: c.courseID, courseTitle: c.title, courseCredits: c.credits })}>
                                            Redaguoti
                                        </Button>

                                        <Button className="mr-2" onClick={() => this.deleteCourse(c.courseID, c.title)} variant="danger">
                                            Ištrinti
                                        </Button>

                                        <EditCourseModal show={this.state.editModalShow} onHide={editModalClose}
                                            courseId={courseId}
                                            courseTitle={courseTitle}
                                            courseCredits={courseCredits}
                                        />
                                    </ButtonToolbar>
                                </td>
                            </tr>
                        )}
                    </tbody>
                </Table>
                <ButtonToolbar>
                    <Button variant="primary" onClick={() => this.setState({ addModalShow: true })}>
                        Naujas kursas
                    </Button>
                    <AddCourseModal show={this.state.addModalShow} onHide={addModalClose} />
                </ButtonToolbar>
            </div>
        );
    }
}