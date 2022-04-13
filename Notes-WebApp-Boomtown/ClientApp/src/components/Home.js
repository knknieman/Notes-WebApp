import React, { Component } from 'react';

import ReactTable from "react-table-6";
import "react-table-6/react-table.css";
import { Link } from 'react-router-dom'

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { noteMetadata: [], loading: true };
    }

    componentDidMount() {
        this.populateNotesData();
    }


    state = { selected: null }
    renderNotesTable(noteMetadata) {
        return (
            <ReactTable ref="table"
                data={noteMetadata}
                columns={[
                    {
                        Header: "Notes",
                        columns: [
                            {
                                Header: "ID",
                                accessor: "noteID",
                                style: { textAlign: "right" }
                            },
                            {
                                Header: "Note Title",
                                accessor: "noteName"
                            },
                            {
                                Header: "Creation Date",
                                accessor: "creationDate"
                            },
                            {
                                Header: "Last Modified",
                                accessor: "lastModified"
                            }
                        ]
                    }
                ]}
                getTrProps={(state, rowInfo) => {
                    if (rowInfo && rowInfo.row) {
                        return {                          
                            onClick: (e) => {
                                this.props.history.push("/note/" + rowInfo.row.noteID);
                            }
                        }
                    } else {
                        return {}
                    }
                }
            }
            />
        );

    
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderNotesTable(this.state.noteMetadata);
            
        return (
            <div>
                <div class='row'>
                    <div class="col-md-12 bg-light text-right" >
                        <h5 id="tabelLabel" style={{ "float": 'left' }} >Please Make a Selection to View Note</h5>
                        <Link className="btn btn-primary m-1" role="button" style={{ "float": 'right' }} to={{ pathname: "/note/create" }}>Create Note</Link>
                    </div>
                </div>
                {contents}
              
            </div>
        );
    }

    async populateNotesData() {
        const response = await fetch('notes');
        const data = await response.json();
        this.setState({ noteMetadata: data, loading: false });
    }
}
