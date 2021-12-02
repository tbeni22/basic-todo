import './App.css';
import {ListView} from "./components/ListView";
import {EditView} from "./components/EditView";
import {Divider} from "@mui/material";
import React from "react";

class App extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            items: []
        }
    }

    saveFunc = () => {

    };

    selectFunc = (data) => {

    };

    removeFunc(id) {
        // todo: remove from db
    }

    moveFunc(id, dir) {
        // todo: update in db
    }

    async getData(id) {
        const response = await fetch('https://jsonplaceholder.typicode.com/todos/' + (id + 1));
        const data = await response.json();

        const currentItems = this.state.items;
        currentItems.concat({title: data.title, state: data.completed ? "done" : "pending"})
        this.setState({ items: currentItems });
    }

    componentDidMount() {
        const list = []
        for (let i = 0; i < 2; i++) {
            list.push({
                title: "Item " + i,
                desc: "This is the description of the todo item.",
                date: "2021.22.22.",
                state: "completed",
                id: i,
                loading: false}
            );
        }
        this.setState({
            items: list
        })

    }

    render() {
        return (
         <div className="App">
            <div id="top-panel">
                <h1>Tasks to do</h1>
                <EditView saveHandler={this.saveFunc.bind(this)}/>
                <Divider variant="middle"/>
            </div>
            <ListView items={this.state.items}
                      selectHandler={(data) => this.selectFunc(data)}
                      removeHandler={(id) => this.removeFunc(id)}
                      moveHandler={(id, dir) => this.moveFunc(id, dir)}
            />
        </div> );
    }
}

export default App;
