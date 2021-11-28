import React from "react";
import {Card, CircularProgress} from "@mui/material";

export function TodoItem(props) {
    function renderBody() {
        return (
            <div>
                <p className="item-name">{props.name}</p>
                <p className="item-desc">{props.desc}</p>
                <div className="item-prop-container">
                    <span className="item-date">{props.date}</span>
                    <span className="item-state">{props.state}</span>
                </div>
            </div>
        );
    }

    return (
        <Card className={props.selected ? "todo-item selected" : "todo-item"} onClick={() => props.onSelect(props.id)}>
            { props.loading ?
                <CircularProgress className="loading-bar"/>
                : renderBody()
            }
        </Card>
    );
}