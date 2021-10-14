import React from "react";
import {TodoItem} from "./TodoItem";
import {Button, CircularProgress} from '@mui/material';

import "./ItemList.css";

export class ListView extends React.Component {
    constructor(props) {
        super(props);
        this.handleSelect = props.selectHandler;
        this.state = {
            numOfItems: props.num ? props.num : 0,
            selected: null,
            loading: false,
            items: [
                {name: "Task 1"}, {name: "Task 2"}
            ],
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
        this.handleSelect(id);
    }

    render() {
        let list = [];
        for (var i = 0; i < this.state.numOfItems; i++) {
            var item = this.state.items[i];
            list.push(<TodoItem name={item != null ? item.name : "name"}
                                desc="This is the description of the todo item."
                                date="2021.22.22."
                                state="completed"
                                key={i}
                                id={i}
                                onSelect={(id) => this.selectItem(id)}
                                selected={ this.state.selected === i }
                                loading={ false }
            />);
        }

        /*<Skeleton className="loading-bar" variant="rectangular" width={420} height={60}/>*/

        return (
            <div className="list-container">
                { (this.state.loading) ?
                    <CircularProgress className="loading-bar"/>
                    : <div className="item-list">{list}</div> }
                <Button variant="contained" id="add-btn"
                        onClick={() => this.addItem()}>
                    +
                </Button>
            </div>
        )
    }
}