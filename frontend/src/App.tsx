import React, { useState } from 'react';
import { ToastContainer, toast } from 'react-toastify';

function FetchButton(props: { title: string, method?: "POST" | "DELETE", url: string, onData: (data: any) => void, children: React.ReactNode }) {
  return <button type="button" className="btn btn-primary" onClick={() => {
    const t = toast(props.title + "...", { isLoading: true })
    fetch(props.url, { method: props.method }).then(r => r.json())
      .then(data => {
        toast.update(t, {
          render: props.title,
          isLoading: false,
          type: 'success',
          autoClose: 5000
        })
        props.onData(data);
      }).catch(e => toast.update(t, {
        render: props.title,
        type: 'error',
        isLoading: false,
        autoClose: 5000
      }))
  }}>{props.children}</button>
}

function DoubleRead() {
  const [values, setValues] = useState([] as number[])
  return <>
    <h2> Double Read</h2>
    <FetchButton title='Double Read' url="/dbTest/doubleRead" onData={setValues}>Load</FetchButton>
    {values.length == 0 ? null : <>
      <h3> Values</h3>
      <ul>
        {values.map((f, idx) => <li key={idx}>{f}</li>)}
      </ul></>
    }
  </>
}
interface TestEntity {
  id: number,
  value: number,
}
function App() {
  const [values, setValues] = useState([] as TestEntity[])
  const [delay, setDelay] = useState(false);
  return (
    <div className="container">
      <h1> TX Demo</h1>
      <FetchButton title='Load' url="/dbTest" onData={setValues}>Load</FetchButton>

      <FetchButton title='Sum and Insert' url={`/dbTest?delay=${delay}`} method="POST" onData={setValues}>Sum and Insert</FetchButton>

      <FetchButton title='Clear' url="/dbTest" method="DELETE" onData={setValues}>Clear</FetchButton>

      <br />

      <div className="form-check" onClick={() => setDelay(x => !x)}>
        <input className="form-check-input" type="checkbox" checked={delay} onChange={() => { }} />
        <label className="form-check-label">
          Delay
        </label>
      </div>

      {values.length == 0 ? null : <>
        <h3> Values</h3>
        <ul>
          {values.map((f, idx) => <li key={idx}>Id: {f.id} Value: {f.value}</li>)}
        </ul></>
      }
      <DoubleRead />
    </div>
  );
}

export default App;
