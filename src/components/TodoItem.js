import React from "react";

export function TodoItem(props) {
    return (
        <div className="todo-item">
            <p className="item-name">{props.name}</p>
            <p className="item-desc">{props.desc}</p>
            <div className="item-prop-container">
                <span className="item-date">{props.date}</span>
                <span className="item-state">{props.state}</span>
            </div>
        </div>
    );
}