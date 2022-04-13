import React, { Component } from 'react';

export class EditNote extends Component {
    constructor(props) {
        super(props);
        const _id = this.props.id;
        const _mode = this.props.mode;
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.createForm = this.createForm.bind(this);
        this.createButtons = this.createButtons.bind(this);
        this.deleteEntry = this.deleteEntry.bind(this);

        this.state = {
            id: _id,
            mode:_mode,
            noteData: {},
            loading : true
        }
    }

    componentDidMount() {
        //Load Passed ID into API, otherwise start with fresh Form
        if (this.state.mode === "Edit") {
            this.getNoteData(this.state.id);
        }
    }


    handleSubmit(evt) {
        evt.preventDefault();
        this.apiCall = "";
        if (this.state.mode === "Edit") {
            this.apiCall = "PUT";
        } else if (this.state.mode === "Create") {
            this.apiCall = "POST"
        }

        fetch('/notes', {
            method: this.apiCall,
            headers: { "content-type": "application/json" },
            body: JSON.stringify(this.state.noteData),
        }).then((response) => response.json())
            .then((responseJson) => {
                if (responseJson === 200) {
                    //Redirect home if successful
                    window.location = "/";
                } else {
                    //Alert User Submit Failed
                    window.alert("Submit Failed with ReturnCode: " + responseJson);
                    window.location.reload(false);
                }
            });


    }

    handleChange(evt) {
        this.setState(prevState => ({
            noteData: {                   
                ...prevState.noteData,    
                [evt.target.name]: evt.target.value      
            }
        }));
    }

    deleteEntry(evt) {
        evt.preventDefault();
        if (window.confirm("Are you sure you want to delete: " + this.state.noteData.noteName)) {
            fetch('/notes/' + this.state.id, {
                method: 'DELETE'
            }).then((response) => response.json())
                .then((responseJson) => {
                    if (responseJson === 200) {
                        window.location = "/";
                    } else {
                        window.alert("Submit Failed with ReturnCode: " + responseJson);
                        window.location = "/";
                    }
                });
        }
    }

    createButtons() {
        if (this.state.mode === "Edit") {
            return (
                <div class='col-xs-3'>
                    <button type='submit' class='btn btn-primary m-1'>Save</button>
                    <button class='btn btn-primary m-1' onClick={this.deleteEntry}>Delete</button>
                </div>
            )
        } else if (this.state.mode === 'Create') {
            return (
                <div class='col-xs-3'>
                    <button type='submit' class='btn btn-primary m-1'>Save</button>
                </div>
            )
        }
    }

    createForm() {
        return (
            <form onSubmit={this.handleSubmit}>
                <div class='form-control'>
                    <label>Note Title</label>
                    <input type="text" class="form-control" name="noteName" defaultValue={this.state.noteData.noteName} onChange={this.handleChange} />

                    <label>Note Content</label>
                    <textarea name="noteContent" class="form-control" rows="3" defaultValue={this.state.noteData.noteContent} onChange={this.handleChange} />
                </div>
                {this.createButtons()}
            </form>
        );
    }

    render() {
        return this.createForm();
    }

    async getNoteData(id) {
        const response = await fetch('notes/' + id);
        const data = await response.json();
        this.setState({ noteData: data, loading: false });
    }
}

