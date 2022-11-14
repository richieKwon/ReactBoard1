import React, {useEffect, useState} from "react";

export default function EntryIndex(props){
    
    const [ status, setStatus ] = useState({entries:[], loading:true});
    useEffect();
    
    function navigateToCreate(){
        const {history} = props
        history.push('entries/create')
    }
    
    
    return(
        <>
            <h1>Article List</h1>
        </>
    );
} 