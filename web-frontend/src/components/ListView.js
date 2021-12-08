import React from "react";
import {TodoItem} from "./TodoItem";
import {CircularProgress} from '@mui/material';

import "./ItemList.css";

export class ListView extends React.Component {
    constructor(props) {
        super(props)
        this.handleSelect = props.selectHandler
        this.handleRemove = props.removeHandler
        this.handleMove = props.moveHandler
        this.items = props.items
        this.state = {}
    }

    selectItem(id) {
        this.setState({
            selected: id
        })  // not necessary anymore
        this.handleSelect(id);
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

        if (newIdx < 0 || newIdx > this.items.length - 1)
            return

        this.handleMove(id, dir)

        let tmp = this.items[idx]
        this.items[idx] = this.items[newIdx]
        this.items[newIdx] = tmp
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
                                 onSelect={(id) => this.selectItem(id)}
                                 selected={this.state.selected === item.id}
                                 loading={false}
                                 deleteHandler={(id) => this.remove(id)}
                                 moveHandler={(id, dir) => this.move(id, dir)}
                        />
        })

        return (
            <div className="list-container">
                { (this.props.loading) ?
                    <CircularProgress className="loading-bar"/>
                    : <div className="item-list">{list}</div> }
            </div>
        )
    }
}