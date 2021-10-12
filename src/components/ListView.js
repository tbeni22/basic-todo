import React from "react";
import {TodoItem} from "./TodoItem";
import Button from '@mui/material/Button';

import "./ItemList.css";

export class ListView extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            numOfItems: props.num ? props.num : 0,
            selected: null
        };
    }

    /*async getData(id) {
        const response = await fetch('https://jsonplaceholder.typicode.com/todos/' + id);
        const data = await response.json();
        this.setState({ item: data });
    }*/

    addItem() {
        let current = this.state.numOfItems;
        this.setState({
            numOfItems: current + 1
        });
    }

    selectItem(id) {
        this.setState({
            selected: id
        })
        //console.log("selected: " + id)
    }

    render() {
        let list = [];
        for (var i = 0; i < this.state.numOfItems; i++) {
            list.push(<TodoItem name="name"
                                desc="This is the description of the todo item."
                                date="2021.22.22."
                                state="completed"
                                key={i}
                                id={i}
                                onSelect={(id) => this.selectItem(id)}
                                selected={ this.state.selected === i }
            />);
        }

        return (
            <div>
                <div className="item-list">{list}</div>
                <Button variant="contained" id="add-btn" onClick={() => this.addItem()}>+</Button>
            </div>
        )
    }
}