import './App.css';
import {ListView} from "./components/ListView";
import {EditView} from "./components/EditView";
import {Divider} from "@mui/material";

function App() {
    const saveFunc = () => {

    };

    const selectFunc = (id) => {

    };

    return (
    <div className="App">
        <div id="top-panel">
            <h1>Tasks to do</h1>
            <EditView saveHandler={saveFunc}/>
            <Divider variant="middle"/>
        </div>
        <ListView num={2} selectHandler={(data) => selectFunc(data)}/>
    </div>
  );
}

export default App;
