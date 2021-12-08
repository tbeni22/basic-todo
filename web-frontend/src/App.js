import './App.css';
import {ListView} from "./components/ListView";
import {EditView} from "./components/EditView";
import {EditDialog} from "./components/EditDialog";
import {Card, Divider} from "@mui/material";
import React from "react";

class App extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            items: [],
            editDialogOpen: false
        }
        this.selectedItem = null
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

    convertToDto(item) {
        return {
            id: item.id,
            title: item.title,
            description: item.desc,
            deadline: item.date != null ? new Date (item.date).toISOString() : null,
            categoryName: item.itemState
        }
    }

    addItemHandler(newItem) {
        const newTodo = this.convertToDto(newItem)
        this.addItem(newTodo).then(id => {
            newTodo.id = id
            this.setState({ items: this.state.items.concat([newTodo])})
        })
    };

    closeEditDialog() {
        this.setState({
            editDialogOpen: false
        })
    }

    updateItemHandler(item) {
        this.closeEditDialog()
        let data = this.convertToDto(item)
        this.updateItem(data)
            .then(() => {
                let idx = this.state.items.findIndex(i => i.id === data.id)
                const arr = this.state.items
                arr.splice(idx, 1, data)
                this.setState({items: arr})
            })
    }

    async updateItem(item) {
        const response = await fetch("http://localhost:5000/todos/" + item.id, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(item)
        })
        if (!response.ok)
            throw Error("Post failed")
    }

    selectItemHandler(id) {
        this.selectedItem = this.state.items.find(item => item.id === id)
        this.setState({editDialogOpen: true})
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
                <Card id="edit-panel" className="todo-item">
                    <EditView saveHandler={this.addItemHandler.bind(this)}/>
                </Card>
                <Divider variant="middle"/>
            </div>
            <ListView items={this.state.items}
                      loading={this.state.loading}
                      selectHandler={(data) => this.selectItemHandler(data)}
                      removeHandler={(id) => this.removeItemHandler(id)}
                      moveHandler={(id, dir) => this.moveItemHandler(id, dir)}
            />
            <EditDialog isOpen={this.state.editDialogOpen}
                        saveHandler={this.updateItemHandler.bind(this)}
                        handleClose={this.closeEditDialog.bind(this)}
                        item={this.selectedItem}
            />
        </div> );
    }
}

export default App;
