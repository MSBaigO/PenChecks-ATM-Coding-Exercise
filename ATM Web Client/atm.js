// Base URL for API requests
const BASE_URL = 'http://localhost:5287/api/ATM';

/**
 * Shows the form corresponding to the selected transaction type.
 */
function showTransactionForm() {
    // Get the selected transaction type from the dropdown
    const transactionType = document.getElementById('transaction-type').value;

    // An array of all form IDs
    const forms = ['deposit-form', 'withdraw-form', 'transfer-form', 'transactions-form', 'account-info-form'];

    // Hide all forms by adding the 'hidden' class
    forms.forEach(formId => {
        document.getElementById(formId).classList.add('hidden');
    });

    // Show the selected form by removing the 'hidden' class
    document.getElementById(transactionType + '-form').classList.remove('hidden');
}

/**
 * Displays formatted response in the output area
 * @param {Object} response - The response object from the API
 * @param {boolean} isError - Whether this is an error response
 */
function displayResponse(response, isError = false) {
    const output = document.getElementById("output");

    if (isError) {
        output.innerHTML = `<div style="color: red; font-weight: bold;">Error: ${response}</div>`;
    } else {
        // Format success responses based on response type
        if (response.accountId !== undefined && response.balance !== undefined) {
            // AccountBalanceResponse
            output.innerHTML = `
                <div style="color: green; font-weight: bold;">Transaction Successful!</div>
                <div>Account ID: ${response.accountId}</div>
                <div>New Balance: $${response.balance.toFixed(2)}</div>
            `;
        } else if (response.success !== undefined) {
            // TransferResponse
            const color = response.success ? 'green' : 'red';
            output.innerHTML = `
                <div style="color: ${color}; font-weight: bold;">${response.message}</div>
                <div>Status: ${response.success ? 'Success' : 'Failed'}</div>
            `;
        } else {
            // Fallback for other response types
            output.innerHTML = `<div style="color: green;">Response: ${JSON.stringify(response, null, 2)}</div>`;
        }
    }
}

/**
 * Handles API errors and displays appropriate error messages
 * @param {Response} response - The fetch response object
 */
async function handleApiError(response) {
    try {
        const errorData = await response.json();

        // Check if it's a TransferResponse with error details
        if (errorData.success === false && errorData.message) {
            displayResponse(errorData.message, true);
        } else if (typeof errorData === 'string') {
            displayResponse(errorData, true);
        } else {
            displayResponse(errorData.message || 'An unknown error occurred', true);
        }
    } catch (parseError) {
        // If response is not JSON, get text
        const errorText = await response.text();
        displayResponse(errorText || 'An unknown error occurred', true);
    }
}

/**
 * Validates numeric input and shows error if invalid
 * @param {number} value - The value to validate
 * @param {string} fieldName - Name of the field for error messages
 * @returns {boolean} - True if valid, false if invalid
 */
function validateNumericInput(value, fieldName) {
    if (isNaN(value) || value <= 0) {
        displayResponse(`${fieldName} must be a positive number`, true);
        return false;
    }
    return true;
}

/**
 * Handles the deposit transaction by making a POST request to the API.
 */
async function deposit() {
    try {
        // Get and validate the account ID and amount from the input fields
        const accountId = parseInt(document.getElementById('deposit-account').value);
        const amount = parseFloat(document.getElementById('deposit-amount').value);

        // Validate inputs
        if (!validateNumericInput(accountId, 'Account ID') || !validateNumericInput(amount, 'Amount')) {
            return;
        }

        // Create a DepositRequest object for the API request body
        const depositRequest = {
            accountId: accountId,
            amount: amount
        };

        // Send a POST request to the deposit endpoint
        const response = await fetch(`${BASE_URL}/deposit`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(depositRequest)
        });

        if (response.ok) {
            // Parse and display the AccountBalanceResponse
            const accountBalanceResponse = await response.json();
            displayResponse(accountBalanceResponse);
        } else {
            // Handle API errors
            await handleApiError(response);
        }
    } catch (error) {
        // Log and display any network or parsing errors
        console.error('Error:', error);
        displayResponse('Network error. Please check your connection and try again.', true);
    }
}

/**
 * Handles the withdraw transaction by making a POST request to the API.
 */
async function withdraw() {
    try {
        // Get and validate the account ID and amount from the input fields
        const accountId = parseInt(document.getElementById('withdraw-account').value);
        const amount = parseFloat(document.getElementById('withdraw-amount').value);

        // Validate inputs
        if (!validateNumericInput(accountId, 'Account ID') || !validateNumericInput(amount, 'Amount')) {
            return;
        }

        // Create a WithdrawRequest object for the API request body
        const withdrawRequest = {
            accountId: accountId,
            amount: amount
        };

        // Send a POST request to the withdraw endpoint
        const response = await fetch(`${BASE_URL}/withdraw`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(withdrawRequest)
        });

        if (response.ok) {
            // Parse and display the AccountBalanceResponse
            const accountBalanceResponse = await response.json();
            displayResponse(accountBalanceResponse);
        } else {
            // Handle API errors
            await handleApiError(response);
        }
    } catch (error) {
        // Log and display any network or parsing errors
        console.error('Error:', error);
        displayResponse('Network error. Please check your connection and try again.', true);
    }
}

/**
 * Handles the transfer transaction by making a POST request to the API.
 */
async function transfer() {
    try {
        // Get and validate the "from" account ID, "to" account ID, and amount from the input fields
        const fromAccountId = parseInt(document.getElementById('transfer-from-account').value);
        const toAccountId = parseInt(document.getElementById('transfer-to-account').value);
        const amount = parseFloat(document.getElementById('transfer-amount').value);

        // Validate inputs
        if (!validateNumericInput(fromAccountId, 'From Account ID') ||
            !validateNumericInput(toAccountId, 'To Account ID') ||
            !validateNumericInput(amount, 'Amount')) {
            return;
        }

        // Additional validation for transfer
        if (fromAccountId === toAccountId) {
            displayResponse('Cannot transfer to the same account', true);
            return;
        }

        // Create a TransferRequest object for the API request body
        const transferRequest = {
            fromAccountId: fromAccountId,
            toAccountId: toAccountId,
            amount: amount
        };

        // Send a POST request to the transfer endpoint
        const response = await fetch(`${BASE_URL}/transfer`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(transferRequest)
        });

        if (response.ok) {
            // Parse and display the TransferResponse
            const transferResponse = await response.json();
            displayResponse(transferResponse);
        } else {
            // Handle API errors (which may also be TransferResponse objects)
            await handleApiError(response);
        }
    } catch (error) {
        // Log and display any network or parsing errors
        console.error('Error:', error);
        displayResponse('Network error. Please check your connection and try again.', true);
    }
}

/**
 * Maps transaction type enum values to readable strings
 * @param {number} transactionType - The transaction type enum value (0=Deposit, 1=Withdraw, 2=Transfer)
 * @returns {string} - The readable transaction type string
 */
function getTransactionTypeString(transactionType) {
    const transactionTypes = {
        0: 'Deposit',
        1: 'Withdraw',
        2: 'Transfer'
    };
    return transactionTypes[transactionType] || 'Unknown';
}

/**
 * Fetches and displays all transactions
 */
async function getTransactions() {
    try {
        const response = await fetch(`${BASE_URL}/transactions`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (response.ok) {
            const transactions = await response.json();
            const output = document.getElementById("output");

            if (transactions.length === 0) {
                output.innerHTML = '<div>No transactions found.</div>';
            } else {
                let html = '<div style="font-weight: bold;">Transaction History:</div>';
                transactions.forEach((transaction, index) => {
                    const transactionTypeString = getTransactionTypeString(transaction.type);
                    html += `
                        <div style="margin: 10px 0; padding: 10px; border: 1px solid #ccc;">
                            <div>Transaction ${index + 1}</div>
                            <div>Type: ${transactionTypeString}</div>
                            <div>Amount: ${transaction.amount.toFixed(2)}</div>
                            <div>Date: ${new Date(transaction.timestamp).toLocaleString()}</div>
                            ${transaction.fromAccount ? `<div>From Account: ${transaction.fromAccount}</div>` : ''}
                            ${transaction.toAccount ? `<div>To Account: ${transaction.toAccount}</div>` : ''}
                        </div>
                    `;
                });
                output.innerHTML = html;
            }
        } else {
            await handleApiError(response);
        }
    } catch (error) {
        console.error('Error:', error);
        displayResponse('Network error. Please check your connection and try again.', true);
    }
}

/**
 * Fetches and displays account information
 */
async function getAccount() {
    try {
        const accountId = parseInt(document.getElementById('account-lookup-id').value);

        if (!validateNumericInput(accountId, 'Account ID')) {
            return;
        }

        const response = await fetch(`${BASE_URL}/account/${accountId}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (response.ok) {
            const account = await response.json();
            const output = document.getElementById("output");
            output.innerHTML = `
                <div style="font-weight: bold;">Account Information:</div>
                <div>Account ID: ${account.id}</div>
                <div>Current Balance: $${account.balance.toFixed(2)}</div>
            `;
        } else {
            await handleApiError(response);
        }
    } catch (error) {
        console.error('Error:', error);
        displayResponse('Network error. Please check your connection and try again.', true);
    }
}

// Initialize the form view when the script loads
document.addEventListener('DOMContentLoaded', () => {
    showTransactionForm();
});