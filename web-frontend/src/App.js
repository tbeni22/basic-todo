import './App.css';
import {ListView} from "./components/ListView";
import {EditView} from "./components/EditView";
import {Divider} from "@mui/material";
import React from "react";

class App extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            items: []
        }
    }

    async addItem(newItem) {
        const response = await fetch("http://localhost:5000/todos/", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(newItem)
        })
        const data = await response.json()
        if (response.ok)
            return data.id;
        else {
            throw Error("Post failed")
        }
    }

    addItemHandler(newItem) {
        let date = new Date (newItem.date)
        let newTodo = {
            title: newItem.title,
            description: newItem.desc,
            deadline: date.toISOString(),
            categoryName: newItem.itemState
        }
        this.addItem(newTodo).then(id => {
            newTodo.id = id
            this.setState({ items: this.state.items.concat([newTodo])})
        })
    };

    selectFunc = (data) => {

    };

    async deleteItem(id) {
        const response = await fetch("http://localhost:5000/todos/" + id, {
            method: 'DELETE'
        })
        return response.status
    }

    removeItemHandler(id) {
        this.deleteItem(id)
            .then(code => {
                console.log(code)
            })
    }

    moveItemHandler(id, dir) {
        // todo: update in db
    }

    async getList() {
        const response = await fetch("http://localhost:5000/todos/");
        const data = await response.json();
        this.setState({ items: data });
        return response.status === 200
    }

    componentDidMount() {
        this.getList().then(success => {
            if (success) {
                this.setState({loading: false})
                console.log("list loaded")
            }
        })
    }

    render() {
        return (
         <div className="App">
            <div id="top-panel">
                <h1>Tasks to do</h1>
                <EditView saveHandler={this.addItemHandler.bind(this)}/>
                <Divider variant="middle"/>
            </div>
            <ListView items={this.state.items}
                      loading={this.state.loading}
                      selectHandler={(data) => this.selectFunc(data)}
                      removeHandler={(id) => this.removeItemHandler(id)}
                      moveHandler={(id, dir) => this.moveItemHandler(id, dir)}
            />
        </div> );
    }
}

export default App;
