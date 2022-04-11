import React, { Component } from 'react';

// Import React Table
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
                        Header: "Test",
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
                            },
                            {
                                Header: "Path on Disk",
                                accessor: "pathOnDisk"
                            }
                        ]
                    }
                ]}
                getTrProps={(state, rowInfo) => {
                    if (rowInfo && rowInfo.row) {
                        console.log(rowInfo);
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
                <h1 id="tabelLabel" >Notes</h1>
                <p>Please Make a Selection to View Note</p>
                <Link className="btn btn-primary " role="button" to={{ pathname: "/note/create" }}>Create New Note</Link>
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
