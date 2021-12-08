import React from "react";
import {TodoItem} from "./TodoItem";
import {Alert, AlertTitle, CircularProgress} from '@mui/material';

import "./ItemList.css";

export class ListView extends React.Component {
    constructor(props) {
        super(props)
        this.handleEdit = props.editHandler
        this.handleRemove = props.removeHandler
        this.handleMove = props.moveHandler
        this.items = props.items
        this.state = {}
    }

    edit(id) {
        this.handleEdit(id);
    }

    idCheck(item, id) {
        return item.id === id
    }

    remove(id) {
        this.handleRemove(id) // remove from db
        let idx = this.items.findIndex((item) => this.idCheck(item, id))
        this.items.splice(idx, 1)
    }

    move(id, dir) {

        let idx = this.items.findIndex((item) => this.idCheck(item, id))
        let newIdx = idx + dir

        // prevent
        if (newIdx < 0 || newIdx > this.items.length - 1)
            return

        this.handleMove(idx, dir) // update in db and locally
    }

    render() {
        this.items = this.props.items
        let list = this.items.flatMap((item) => {
            if (typeof(item) == "undefined") return [];
            if (item == null || item.loading)
                return <TodoItem loading={true}/>; // key?
            else
                return <TodoItem title={item.title}
                                 desc={item.description}
                                 date={new Date(item.deadline).toLocaleDateString()}
                                 state={item.categoryName}
                                 key={item.id}
                                 id={item.id}
                                 editHandler={(id) => this.edit(id)}
                                 loading={false}
                                 deleteHandler={(id) => this.remove(id)}
                                 moveHandler={(id, dir) => this.move(id, dir)}
                        />
        })

        const renderLoading = (isLoading) => {
            if (isLoading == null) {
                return (
                    <Alert severity="error" id="list-load-error-msg" className="list-substitute">
                        <AlertTitle>Error</AlertTitle>
                        Failed to load list from server. Check your internet connection and reload the page.
                    </Alert>
                )
            }
            else if (isLoading) {
                return <CircularProgress className="list-substitute"/>
            }
        }

        return (
            <div className="list-container">
                {
                    (this.props.loading === false) ?
                        <div className="item-list">{list}</div>
                    : renderLoading(this.props.loading)
                }
            </div>
        )
    }
}