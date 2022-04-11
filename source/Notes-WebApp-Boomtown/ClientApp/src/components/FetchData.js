import React, { Component } from 'react';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { noteMetadata: [], loading: true };
  }

  componentDidMount() {
    this.populateWeatherData();
  }

    static renderNotesTable(noteMetadata) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            {/*<th>Note ID</th>*/}
            <th>Note Title</th>
            <th>Creation Date</th>
            <th>Last Modified</th>
            <th>Path on Disk</th>
          </tr>
        </thead>
        <tbody>
          {noteMetadata.map(noteMetadata =>
              <tr key={noteMetadata.noteID}>
                  {/* <td>{noteMetadata.noteID}</td>*/}
                  <td>{noteMetadata.noteName}</td>
                  <td>{noteMetadata.creationDate}</td>
                  <td>{noteMetadata.lastModified}</td>
                  <td>{noteMetadata.pathOnDisk}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
        : FetchData.renderNotesTable(this.state.noteMetadata);

    return (
      <div>
        <h1 id="tabelLabel" >Notes</h1>
        <p>Please Make a Selection to View Note</p>
        {contents}
      </div>
    );
  }

  async populateWeatherData() {
    const response = await fetch('weatherforecast');
      const data = await response.json();
    this.setState({ noteMetadata: data, loading: false });
  }
}
