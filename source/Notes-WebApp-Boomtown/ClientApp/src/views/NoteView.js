import React from 'react';
import { useParams } from 'react-router-dom'
import { EditNote } from '../components/EditNote';

const NoteView = (props) => {
    const { id } = useParams();
    return (
        <EditNote id={id} mode="Edit"  />
    )
}
export default NoteView;