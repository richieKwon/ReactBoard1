
import React from "react";

export default function EntryDelete(props) {

    function navigateToIndex(){
        const {history} = props
        history.push('entries/index')
    }

    return (
        <>
            <h3>EntryDelete</h3>
        </>
    );
}