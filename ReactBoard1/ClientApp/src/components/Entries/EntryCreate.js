import React, {useState} from "react";

export default function EntryCreate(props) {
    const [status, setStatus] = useState({
        id : 0,
        name:"",
        title:"",
        content :"",
        created : null
    });

    function navigateToIndex(){
        const {history} = props
        history.push('entries/index')
    }
       
    return (
        <>
            <h3>Entry create page</h3>
        </>
    );
}